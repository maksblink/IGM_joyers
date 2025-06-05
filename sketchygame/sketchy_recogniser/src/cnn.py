import torch
import torch.nn as nn
import torch.nn.functional as F

class SketchyCNN(nn.Module):
    """Convolutional Neural Network for sketch recognition.

        Architecture consists of:
        - 3 convolutional layers with ReLU activation
        - 2 max-pooling layers
        - 3 fully connected layers
        - Dropout layer for regularization

        Input shape: 1x64x64 (grayscale)
        Output shape: 10 (number of classes)
        """

    def __init__(self, *args, **kwargs):
        """Initialize the CNN layers and parameters.

                Layers:
                - conv1: 32 filters, 5x5 kernel
                - pool1: 2x2 max pooling
                - conv2: 64 filters, 3x3 kernel
                - pool2: 2x2 max pooling
                - conv3: 256 filters, 3x3 kernel
                - fc1: 12*12*256 → 512
                - fc2: 512 → 128
                - fc3: 128 → 10 (output)
                - dropout1: p=0.5
                """
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
        """Forward pass through the network.

               Args:
                   x (torch.Tensor): Input tensor of shape (batch_size, 1, 64, 64)

               Returns:
                   torch.Tensor: Output tensor of shape (batch_size, 10)

               Operations:
                   1. Conv1 → ReLU → Pool1
                   2. Conv2 → ReLU → Pool2
                   3. Conv3 → ReLU
                   4. Flatten
                   5. FC1 → ReLU → Dropout
                   6. FC2 → ReLU
                   7. FC3 (output)
               """
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
