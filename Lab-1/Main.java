package org.example;

// 0 - No sorting
// 1 - Sorting Naturally
// 2 - Sorting by Level

public class Main {
    public static void main(String[] args) {
        if(args.length > 0) {
            ApplicationManager aM = new ApplicationManager(args[0]);
            aM.printOutput();
        } else {
            System.out.println("No args given.");
        }
    }
}