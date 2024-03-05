package org.example;

public class ApplicationManager {
    private final MageSetFactory mageSet;
    private final MageStatistics mageStatistics;
    public ApplicationManager(String sortingMode) {
        mageSet = new MageSetFactory(sortingMode);
        mageSet.generateMags();
        mageStatistics = new MageStatistics(mageSet.getMags(), sortingMode);
    }
    public void printOutput() {
        mageSet.printMageSet();
        mageStatistics.printStatistics();
    }
}
