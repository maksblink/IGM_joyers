import socketserver

from src import handler
from path import Path

BASE_FOLDER: str = Path(__file__).parent
ASSETS_FOLDER: str = BASE_FOLDER / "assets"

if __name__ == "__main__":
    """Main server entry point.

       Starts TCP server on localhost:9999

       Uses:
           - RequestHandler for processing requests
           - Serves indefinitely until keyboard interrupt
       """
    try:
        with socketserver.TCPServer(("localhost", 9999), handler.RequestHandler) as server:
            print("Server started!")
            server.serve_forever()
    except KeyboardInterrupt: print("\tServer shutdown")
