package org.example;

import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.Socket;
import java.util.Objects;
import java.util.Scanner;
import java.util.concurrent.TimeUnit;
import java.util.logging.Logger;

public class Client {
    private static final Logger logger = Logger.getLogger(Client.class.getName());

    public static void main(String[] args) {
        try {
            Socket socket = new Socket("127.0.0.1", 1234);
            logger.info("Connected to server: " + socket.getInetAddress());

            ObjectOutputStream outputStream = new ObjectOutputStream(socket.getOutputStream());
            ObjectInputStream inputStream = new ObjectInputStream(socket.getInputStream());

            String readySignal = (String) inputStream.readObject();
            logger.info("Server Ready signal: " + readySignal);

            Scanner scanner = new Scanner(System.in);
            logger.info("Please enter integer number: ");
            int inputNumber = scanner.nextInt();
            outputStream.writeInt(inputNumber);
            outputStream.flush();

            String readyForMessagesSignal = (String) inputStream.readObject();
            logger.info("Server Ready for Messages signal: " + readyForMessagesSignal);

            for (int i = 0; i < inputNumber; i++) {
                logger.info("Input Message Content " + i + ": ");
                String inputM = scanner.nextLine();

                Message message = new Message(i, inputM + (i + 1));
                outputStream.writeObject(message);
                outputStream.flush();

                // TimeUnit.SECONDS.sleep(2);

                String response = (String) inputStream.readObject();
                logger.info("Response from server: " + response);
            }

            String finishedSignal = (String) inputStream.readObject();
            System.out.println("Server Finish signal: " + finishedSignal);
            socket.close();

        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
