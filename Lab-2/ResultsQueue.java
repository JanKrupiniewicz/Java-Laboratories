package org.example;

import java.util.LinkedList;

public class ResultsQueue {
    private final LinkedList<String> results = new LinkedList<>();

    public synchronized void addResult(String task) {
        results.add(task);
    }
    public synchronized String getResult() {
        String writeOutputToFile = "Results of the Calculations: \n";
        for (String result : results) {
            writeOutputToFile = writeOutputToFile + result + "\n";
        }
        return writeOutputToFile;
    }

    public boolean isEmpty() {
        return true;
    }
}