import socketserver

from src import handler
from path import Path

BASE_FOLDER: str = Path(__file__).parent
ASSETS_FOLDER: str = BASE_FOLDER / "assets"

if __name__ == "__main__":
    
    try:
        with socketserver.TCPServer(("localhost", 9999), handler.RequestHandler) as server:
            print("Server started!")
            server.serve_forever()
    except KeyboardInterrupt: print("\tServer shutdown")
