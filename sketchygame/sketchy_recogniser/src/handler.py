import socketserver as ss
import json

from torch import from_numpy, Tensor
from numpy import array, ndarray, uint8
from src.core import SketchyRecognizer

class RequestHandler(ss.StreamRequestHandler):
    """TCP Request Handler for sketch recognition server.

        Processes client requests and returns predictions.

        Attributes:
            cnn (SketchyRecognizer): Shared recognizer instance
        """

    cnn = SketchyRecognizer()

    def handle(self):
        """ Handles request to the server. Uses stream like (file-like objects) communication. Reads rfile with client's data and writes result to wfile. 
        
        Accepts json string like: {"image": [0,0,0,0....]}

        Returns json string like: {"class_id": 0, "class_name": "cls_1"}
        """

        data: str = self.rfile.readline().rstrip()
        image_array: ndarray = array(json.loads(data)["image"], dtype=uint8)
        image_tensor: Tensor = from_numpy(image_array)

        model_responce: dict[str, int|str] = self.cnn.predict_from_array(image_tensor)

        responce: bytes = json.dumps(model_responce).encode() + b'\n'
        self.wfile.write(responce)