import tqdm
import json
import shutil
import random

import PIL.Image
import PIL.ImageDraw
import pandas as pd

from path import Path

# https://www.kaggle.com/datasets/ashishjangra27/doodle-dataset/data?select=master_doodle_dataframe.csv

from path import Path

ASSETS_FOLDER: str = Path(__file__).parent.parent / "assets"
if not ASSETS_FOLDER.exists(): ASSETS_FOLDER.mkdir()

object_list: list[str] = ["house plant", "guitar", "basketball", "sword", "door", "key", "lantern", "chair", "pencil", "axe"]


def filter_data() -> None:
    master_file: Path = ASSETS_FOLDER / "master_doodle_dataframe.csv"
    outmaster_file: Path = ASSETS_FOLDER / "f_master_doodle_dataframe.csv"

    df: pd.DataFrame = pd.read_csv(master_file, index_col=0)

    drop_cols: list[str] = ["countrycode", "key_id", "recognized", "image_path"]
    for col_name in drop_cols:
        if col_name in df.columns:
            df = df.drop(labels=col_name, axis=1)

    df = df[df["word"].isin(object_list)]
    df = df.reset_index(drop=True)

    df.to_csv(outmaster_file)


def draw_strokes(image_draw: PIL.ImageDraw.ImageDraw, strokes: str) -> None:
    for stroke in json.loads(strokes):
        for i in range(0, len(stroke[0]) - 1, 1):
            x1, y1 = stroke[0][i], stroke[1][i]
            x2, y2 = stroke[0][i + 1], stroke[1][i + 1]
            image_draw.line(((x1, y1),(x2, y2)), fill=255, width=2)
    return


def create_images(): 
    master_file: Path = ASSETS_FOLDER / "f_master_doodle_dataframe.csv"
    df: pd.DataFrame = pd.read_csv(master_file)

    CLASSES_FOLDER: Path = ASSETS_FOLDER / "classes"
    if not CLASSES_FOLDER.exists(): CLASSES_FOLDER.mkdir()

    # Future work: add threadpool
    image_number: int = 0
    progres_bar = tqdm.tqdm(desc="Creating Images", total=df.shape[0])
    for image_class, strokes in zip(df["word"], df["drawing"]):
        image_class = str(image_class).translate({ord(" "): ord("_")})

        img: PIL.Image.Image = PIL.Image.new(size=(255, 255), color="black", mode="L")
        image_draw: PIL.ImageDraw.ImageDraw = PIL.ImageDraw.Draw(img)

        draw_strokes(image_draw, strokes)

        class_dir: Path = CLASSES_FOLDER / image_class
        if not class_dir.exists(): class_dir.mkdir()

        img.save(CLASSES_FOLDER / image_class / f"{image_class}_{image_number}.png")
        image_number += 1
        progres_bar.update(1)


def prepare_data() -> None:
    BASE_DIR = Path(__file__).parent.parent
    SOURCE_DIR = BASE_DIR / "assets" / "classes"
    TRAIN_DIR = BASE_DIR / "assets" / "train"
    VALID_DIR = BASE_DIR / "assets" / "valid"

    SPLIT_RATIO = 0.8
    SEED = 42

    random.seed(SEED)

    for target_dir in [TRAIN_DIR, VALID_DIR]:
        if target_dir.exists():
            shutil.rmtree(target_dir)
        target_dir.mkdir()

    for class_folder in SOURCE_DIR.iterdir():
        if class_folder.is_dir():
            images = list(class_folder.glob("*.*"))
            random.shuffle(images)

            split_idx = int(len(images) * SPLIT_RATIO)
            train_images = images[:split_idx]
            valid_images = images[split_idx:]

            train_class_dir = TRAIN_DIR / class_folder.name
            valid_class_dir = VALID_DIR / class_folder.name
            train_class_dir.mkdir()
            valid_class_dir.mkdir()

            for img_path in train_images:
                shutil.copy(img_path, train_class_dir / img_path.name)
            for img_path in valid_images:
                shutil.copy(img_path, valid_class_dir / img_path.name)

            print(f"Class '{class_folder.name}': {len(train_images)} train, {len(valid_images)} valid")

    print("DONE")

if __name__ == "__main__":
    filter_data()
    create_images()
    prepare_data()