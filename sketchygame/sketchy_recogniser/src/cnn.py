import torch
import torch.nn as nn
import torch.nn.functional as F

class SketchyCNN(nn.Module): 

    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)

        self.conv1 = nn.Conv2d(in_channels=1, out_channels=32, kernel_size=5)   # 64x64 → 60x60
        self.pool1 = nn.MaxPool2d(kernel_size=2)                                # 60x60 → 30x30

        self.conv2 = nn.Conv2d(in_channels=32, out_channels=64, kernel_size=3)  # 30x30 → 28x28
        self.pool2 = nn.MaxPool2d(kernel_size=2)                                # 28x28 → 14x14

        self.conv3 = nn.Conv2d(in_channels=64, out_channels=256, kernel_size=3) # 14x14 → 12x12

        self.fc1 = nn.Linear(12*12*256, 512)
        self.fc2 = nn.Linear(512, 128)
        self.fc3 = nn.Linear(128, 10)

        self.dropout1 = nn.Dropout(p=0.5)
        # self.dropout2 = nn.Dropout(p=0.2) # Optional

    def forward(self, x):
        x = self.pool1(F.relu(self.conv1(x)))
        x = self.pool2(F.relu(self.conv2(x)))
        x = F.relu(self.conv3(x))

        x = x.view(-1, 12*12*256)
        
        x = F.relu(self.fc1(x))
        x = self.dropout1(x)
        x = F.relu(self.fc2(x))
        # x = self.dropout2(x) # Optional
        x = self.fc3(x)

        return x
