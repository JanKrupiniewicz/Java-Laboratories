package org.example;

import java.util.LinkedList;
import java.util.List;

public class PrimeRun implements Runnable {
    private TaskQueue taskQueue;
    private ResultsQueue resultsQueue;
    private volatile boolean isRunning = true;

    public PrimeRun(TaskQueue taskQueue, ResultsQueue resultsQueue) {
        this.taskQueue = taskQueue;
        this.resultsQueue = resultsQueue;
    }

    @Override
    public void run() {
        while(isRunning) {
            try {
                int checkPrimeTask = taskQueue.getNextTask();
                String result = checkIfPrime(checkPrimeTask);

                Thread.sleep(10000);

                resultsQueue.addResult(result);
            } catch(Exception e) {
                e.printStackTrace();
            }
        }
    }

    public synchronized void closeProgram() {
        isRunning = false;
        Thread.currentThread().interrupt();
    }

    private String checkIfPrime(int num) {
        String divisorsList = "1";
        for(int i = 2 ; i < num/2 + 1 ; i++) {
            if (num%i == 0) {
                divisorsList = divisorsList + ", " + i;
            }
        }
        if(divisorsList.equals("1")) {
            return num + " IS PRIME";
        } else {
            return num + " NOT PRIME - Divisors: " + divisorsList + ", " + num;
        }
    }
}
