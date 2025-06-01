#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <arpa/inet.h>

#define HOST "127.0.0.1"
#define PORT 9999
#define BUFFER_SIZE 1024

int main() {
    int sock;
    struct sockaddr_in server_address;
    char *data = "{\"image\": [0,0,0,1,1,1,2,2,2]}";
    char buffer[BUFFER_SIZE];

    if ((sock = socket(AF_INET, SOCK_STREAM, 0)) < 0) {
        perror("Socket creation error");
        return EXIT_FAILURE;
    }

    server_address.sin_family = AF_INET;
    server_address.sin_port = htons(PORT);

    if (inet_pton(AF_INET, HOST, &server_address.sin_addr) <= 0) {
        perror("Invalid address/Address not supported");
        close(sock);
        return EXIT_FAILURE;
    }

    if (connect(sock, (struct sockaddr *)&server_address, sizeof(server_address)) < 0) {
        perror("Connection failed");
        close(sock);
        return EXIT_FAILURE;
    }

    if (send(sock, data, strlen(data), 0) < 0) {
        perror("Send failed");
        close(sock);
        return EXIT_FAILURE;
    }

    if (send(sock, "\n", 1, 0) < 0) {
        perror("Send failed");
        close(sock);
        return EXIT_FAILURE;
    }

    printf("Sent:    %s\n", data);

    int bytes_received = recv(sock, buffer, BUFFER_SIZE - 1, 0);
    if (bytes_received < 0) {
        perror("Receive failed");
        close(sock);
        return EXIT_FAILURE;
    }

    buffer[bytes_received] = '\0'; // Null-terminate the received data
    printf("Received: %s\n", buffer);

    close(sock);
    return EXIT_SUCCESS;
}