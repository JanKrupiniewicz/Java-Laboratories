package org.example;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.PrintWriter;
import java.util.LinkedList;
import java.util.List;
import java.util.Scanner;

public class Main {
    public static void main(String[] args) throws FileNotFoundException, InterruptedException {
        TaskQueue taskQueue = new TaskQueue();
        ResultsQueue resultsQueue = new ResultsQueue();
        List<PrimeRun> primeRuns = new LinkedList<>();

        int numberOfThreads = Integer.parseInt(args[0]);
        for (int i = 0; i < numberOfThreads; i++) {
            PrimeRun calculatePrime = new PrimeRun(taskQueue, resultsQueue);
            new Thread(calculatePrime).start();
            primeRuns.add(calculatePrime);
        }

        FileInputStream input = new FileInputStream("ENTER YOU INPUT FILE");
        PrintWriter output = new PrintWriter("ENTER YOU OUTPUT FILE");

        Scanner scanner = new Scanner(System.in);
        Scanner fileScanner = new Scanner(input);


        while(fileScanner.hasNextLine()) {
            String fileInput = fileScanner.nextLine();
            try {
                long checkPrime = Long.parseLong(fileInput);
                taskQueue.addTask(checkPrime);
            } catch (NumberFormatException e) {
                System.out.println("Invalid File input.");
            }
        }

        while(true) {
            System.out.println("Input 'e' for exit, 'r' for showing results or simply type integer number:");
            String task = scanner.nextLine();

            if(task.equalsIgnoreCase("e")) {
                System.out.println("Closing The Application ...");
                for (PrimeRun primeRun : primeRuns) {
                    primeRun.closeProgram();
                }

                output.println(resultsQueue.getResult());
                output.close();
                scanner.close();
                System.exit(0);
            } else if (task.equalsIgnoreCase("r")) {
                System.out.println(resultsQueue.getResult());
            } else {
                try {
                    int checkPrime = Integer.parseInt(task);
                    taskQueue.addTask(checkPrime);
                } catch (NumberFormatException e) {
                    System.out.println("Invalid input.");
                }
            }
        }
    }
}