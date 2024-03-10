package org.example;

import java.util.LinkedList;
import java.util.List;
import java.util.Scanner;

public class Main {
    public static void main(String[] args) {

        Scanner scanner = new Scanner(System.in);
        TaskQueue taskQueue = new TaskQueue();
        ResultsQueue resultsQueue = new ResultsQueue();
        List<PrimeRun> primeRuns = new LinkedList<>();

        int numberOfThreads = Integer.parseInt(scanner.nextLine());
        for (int i = 0 ; i < numberOfThreads; i++) {
            PrimeRun calculatePrime = new PrimeRun(taskQueue, resultsQueue);
            new Thread(calculatePrime).start();
            primeRuns.add(calculatePrime);
        }

        while(true) {
            System.out.println("Input 'e' for exit, 'r' for showing results or simply type integer number:");
            String task = scanner.nextLine();

            if(task.equalsIgnoreCase("e")) {
                System.out.println("Closing The Application ...");

                for (PrimeRun primeRun : primeRuns) {
                    primeRun.closeProgram();
                }

                System.exit(0);
            } else if (task.equalsIgnoreCase("r")) {
                resultsQueue.getResult();
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