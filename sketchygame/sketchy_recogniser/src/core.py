import torch
import tqdm
import numpy as np

from torch import nn, optim
from torchvision import datasets, transforms
from torch.utils.data import DataLoader
from path import Path
from PIL.Image import Image
from random import choice
from os import getcwd
from colorama import Fore
import PIL.Image as pi

from src.cnn import SketchyCNN

try: assert Path(getcwd()).name == "sketchy_recogniser"
except: print(f"{Fore.RED}Invalid CWD -> change to 'sketchy_recogniser'{Fore.WHITE}")

class SketchyRecognizer:

    cnn: SketchyCNN = SketchyCNN()
    device = torch.device('cuda:0' if torch.cuda.is_available() else 'cpu')
    object_list: list[str] = sorted(["house plant", "guitar", "basketball", "sword", "door", "key", "lantern", "chair", "pencil", "axe"])
    train_dir: str = "assets/train"
    valid_dir: str = "assets/valid"
    training_history_file: str = "assets/training_history.csv"
    assest_dir: str = "assets/"

    transform = transforms.Compose([
        transforms.Grayscale(num_output_channels=1),
        transforms.Resize((64, 64)),
        transforms.ToTensor(),
        transforms.Normalize((0.0), (0.5))
        ])
    

    def __init__(self, auto_load_model: bool = True):
        print("Using:\t", SketchyRecognizer.device)

        assets_dir: Path = Path(self.assest_dir)
        training_history_file: Path = Path(self.training_history_file)

        if not training_history_file.exists(): 
            if not assets_dir.exists():
                assets_dir.mkdir()
            training_history_file.touch()
        
        if auto_load_model:
            self.load_model()
        
        SketchyRecognizer.cnn.to(SketchyRecognizer.device)


    def load_model(self, model_name: str = "model.mdl") -> None: 
        try: 
            SketchyRecognizer.cnn.load_state_dict(torch.load(model_name, weights_only=True))
        except: 
            print("Model could not be loaded")


    def save_model(self, model_name: str = "model.mdl") -> None: 
        try:
            SketchyRecognizer.cnn.to("cpu")
            torch.save(SketchyRecognizer.cnn.state_dict(), model_name)
        except: 
            print("Model could not be saved")


    def _predict(self, tensor: torch.Tensor) -> dict[str, int|str]:
        device_tensor = tensor.to(SketchyRecognizer.device)
        prediction: torch.Tensor = SketchyRecognizer.cnn.forward(device_tensor)
        host_prediction = prediction.to("cpu")
        predicted_class: int = host_prediction.flatten().argmax(dim=0).item()
        return {"class_id": predicted_class, "class_name": SketchyRecognizer.object_list[predicted_class]} 


    def test_predict_from_image(self) -> None: 
        images_path: Path = Path(self.train_dir)
        classes: list[Path] = [folder for folder in images_path.iterdir()]
        random_class: str = choice(classes)
        random_image_path: Path = choice([x for x in random_class.iterdir()])
        image: pi.Image = pi.open(random_image_path)
        
        input_tensor = SketchyRecognizer.transform(image)
        prediction: dict[str, int|str] = self._predict(input_tensor)
        print(prediction)
        image.show()
        return


    def predict_from_image(self, image: Image) -> dict[str, int|str]:
        input_tensor: torch.Tensor = SketchyRecognizer.transform(image)
        return self._predict(input_tensor)


    def predict_from_array(self, array: np.ndarray) -> dict[str, int|str]:
        input_tensor = transforms.ToTensor()(np.array(array))
        input_tensor = transforms.Resize((64, 64))(input_tensor)
        input_tensor = transforms.Normalize((0.0), (0.5))(input_tensor)
        return self._predict(input_tensor)


    def train_one_epoch(self, batch_size: int = 64, train_loader = None, optimizer = None, loss_fn = None) -> float:
        SketchyRecognizer.cnn.train()
        running_loss = 0.0

        if train_loader is None: 
            train_dataset: datasets.ImageFolder = datasets.ImageFolder(self.train_dir, transform=self.transform)
            train_loader: DataLoader = DataLoader(train_dataset, batch_size=batch_size, shuffle=True)

        if optimizer is None: 
            optimizer = torch.optim.Adam(SketchyRecognizer.cnn.parameters())

        if loss_fn is None: 
            loss_fn = torch.nn.CrossEntropyLoss()

        # print(train_dataset.classes)
        progress_bar = tqdm.tqdm(desc="Proccessing training dataset...", total=len(train_loader), unit=" batches", colour="green")

        for images, labels in train_loader:
            d_images: torch.Tensor = images.to(self.device)
            d_labels: torch.Tensor = labels.to(self.device)

            optimizer.zero_grad()
            outputs = SketchyRecognizer.cnn(d_images)
            loss = loss_fn(outputs, d_labels)
            loss.backward()
            optimizer.step()

            running_loss += loss.item()
            progress_bar.update(1)

        return running_loss / len(train_loader) 


    def validate(self, batch_size: int = 64, valid_loader = None, validate_data = None, loss_fn = None) -> tuple[float, float]:
        SketchyRecognizer.cnn.eval()
        running_loss = 0.0
        correct = 0
        total = 0

        if valid_loader is None: 
            valid_dataset = datasets.ImageFolder(self.valid_dir, transform=SketchyRecognizer.transform)
            valid_loader = DataLoader(valid_dataset, batch_size=batch_size, shuffle=False)

        if loss_fn is None: 
            loss_fn = torch.nn.CrossEntropyLoss()

        with torch.no_grad():
        
            progress_bar = tqdm.tqdm(desc="Proccessing training dataset...", total=len(valid_loader), unit=" batches", colour="green")

            for images, labels in valid_loader:
                d_images: torch.Tensor = images.to(self.device)
                d_labels: torch.Tensor  = labels.to(self.device)

                outputs: torch.Tensor = SketchyRecognizer.cnn(d_images)
                loss = loss_fn(outputs, d_labels)
                running_loss += loss.item()

                predicted = outputs.to("cpu").argmax(dim=1)
                correct += (predicted == labels).sum().item()
                total += labels.size(0)

                progress_bar.update(1)

        val_loss = running_loss / len(valid_loader)
        val_acc = correct / total * 100
        return val_loss, val_acc


    def train(self, epochs: int = 12, batch_size: int = 64, learning_rate: float = 0.001) -> None:
        train_dir = "assets/train"
        valid_dir = "assets/valid"

        train_dataset = datasets.ImageFolder(train_dir, transform=SketchyRecognizer.transform)
        valid_dataset = datasets.ImageFolder(valid_dir, transform=SketchyRecognizer.transform)

        train_loader = DataLoader(train_dataset, batch_size=batch_size, shuffle=True)
        valid_loader = DataLoader(valid_dataset, batch_size=batch_size, shuffle=False)

        criterion = nn.CrossEntropyLoss()
        optimizer = optim.Adam(SketchyRecognizer.cnn.parameters(), lr=learning_rate)

        traning_history_file = open(self.training_history_file, "a")

        for epoch in range(epochs):
            print('\nEpoch {}/{}\n'.format(epoch, epochs - 1) + '-' * 10)
            train_loss = self.train_one_epoch(train_loader=train_loader, optimizer=optimizer, loss_fn=criterion)
            val_loss, val_acc = self.validate(valid_loader=valid_loader, loss_fn=criterion)
            traning_history_file.write(f"{train_loss},{val_loss},{val_acc}\n")

        traning_history_file.close()
        self.save_model()
