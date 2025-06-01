import socket
import sys
import json
import numpy as np
import PIL.Image as pi

HOST, PORT = "localhost", 9999
# data = "".join(sys.argv[1:])
data: dict[str, list]= {"image": np.array(pi.open("./assets/train/guitar/guitar_18000.png")).tolist()}
res: str = json.dumps(data)

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as sock:
    sock.connect((HOST, PORT))
    sock.sendall(bytes(res, "utf-8"))
    sock.sendall(b"\n")
    received = str(sock.recv(1024), "utf-8")

print("Sent:    ", res)
print("Received:", received)