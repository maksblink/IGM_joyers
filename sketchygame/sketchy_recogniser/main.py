import socketserver

from src import handler
from path import Path

BASE_FOLDER: str = Path(__file__).parent
ASSETS_FOLDER: str = BASE_FOLDER / "assets"

host: str = "localhost"
port: int = 9999

if __name__ == "__main__":
    
    try:
        with socketserver.TCPServer((host, port), handler.RequestHandler) as server:
            print("Server started!")
            server.serve_forever()
    except KeyboardInterrupt: print("\tServer shutdown")
