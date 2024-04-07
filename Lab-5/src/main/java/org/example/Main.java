package org.example;


import java.util.Scanner;

public class Main {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        MageRepository mageRepository = new MageRepository();
        MageController mageController = new MageController(mageRepository);
        System.out.println("Input values [save, find, delete, exit]: ");

        while(true) {
            String input = scanner.nextLine();
            String outputMessage = "";
            String commands[] = input.split(" ");
            if (commands[0].equalsIgnoreCase("save")) {
                outputMessage = mageController.save(commands[1], commands[2]);
            } else if (commands[0].equalsIgnoreCase("find")) {
                outputMessage = mageController.find(commands[1]);
            } else if (commands[0].equalsIgnoreCase("delete")) {
                outputMessage = mageController.delete(commands[1]);
            } else if (commands[0].equalsIgnoreCase("exit")) {
                break;
            }
            System.out.println(outputMessage);
        }
        System.out.println("Closing program ...");
        scanner.close();
    }
}