package org.example;

import java.util.LinkedList;

public class ResultsQueue {
    private LinkedList<String> results = new LinkedList<>();

    public synchronized void addResult(String task) {
        results.add(task);
    }
    public synchronized void getResult() {
        System.out.println("Results of the Calculations: ");
        for (String result : results) {
            System.out.println(result);
        }
    }
}