package org.example;

import java.util.LinkedList;

public class TaskQueue {
    private final LinkedList<Long> tasks = new LinkedList<>();

    public synchronized void addTask(long task) {
        tasks.add(task);
        notify();
    }

    public synchronized Long getNextTask() throws InterruptedException {
        while (tasks.isEmpty()) {
            wait();
        }
        return tasks.poll();
    }
}
