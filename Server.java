package org.example;

import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.logging.Level;
import java.util.logging.Logger;

public class Server {
    private static final Logger logger = Logger.getLogger(Server.class.getName());

    public static void main(String[] args) {
        try {
            ServerSocket serverSocket = new ServerSocket(1234);
            logger.info("Server started. Waiting for clients to connect.");

            while(true) {
                Socket clientSocket = serverSocket.accept();
                logger.info("Client connected: " + clientSocket.getInetAddress());

                Thread thread = new Thread(new ServerThread(clientSocket));
                thread.start();
            }

        } catch (Exception e) {
            e.printStackTrace();
            logger.log(Level.SEVERE, "Server exception", e);
        }
    }

    static class ServerThread implements Runnable {
        private final Socket clientSocket;

        public ServerThread(Socket socket) {
            this.clientSocket = socket;
        }

        @Override
        public void run() {
            try {
                ObjectOutputStream outputStream = new ObjectOutputStream(clientSocket.getOutputStream());
                ObjectInputStream inputStream = new ObjectInputStream(clientSocket.getInputStream());

                outputStream.writeObject("ready");

                int inputN = inputStream.readInt();
                logger.info("Received number from client: " + inputN);

                outputStream.writeObject("ready for messages");

                for(int i = 0 ; i < inputN; i++) {
                    try {
                        Message message = (Message) inputStream.readObject();
                        logger.info("Received message from client: " + message.getContent());

                        outputStream.writeObject("Message received: " + message.getContent());
                        outputStream.flush();
                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                }
                outputStream.writeObject("finished");

            } catch (Exception e) {
                logger.log(Level.SEVERE, "Client handler exception", e);
            } finally {
                try {
                    clientSocket.close();
                } catch (IOException e) {
                    logger.log(Level.SEVERE, "Error closing client socket", e);
                }
            }
        }

    }
}
