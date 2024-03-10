package org.example;

import java.util.LinkedList;

public class TaskQueue {
    private LinkedList<Integer> tasks = new LinkedList<>();

    public synchronized void addTask(int task) {
        tasks.add(task);
        notify();
    }

    public synchronized int getNextTask() throws InterruptedException {
        while (tasks.isEmpty()) {
            wait();
        }
        return tasks.poll();
    }
}
