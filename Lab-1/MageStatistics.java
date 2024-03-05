package org.example;
import java.util.*;

public class MageStatistics {
    private final Map<Mage, Integer> statisticsMap;
    public MageStatistics() {
        statisticsMap = null;
    }
    public MageStatistics(Set<Mage> mageSet, String sortingMode) {
        if ("noSorting".equals(sortingMode) || "0".equals(sortingMode) ) {
            statisticsMap = new HashMap<>();
        } else if ("naturalOrder".equals(sortingMode) || "1".equals(sortingMode)) {
            statisticsMap = new TreeMap<>();
        } else if ("alternativeOrder".equals(sortingMode) || "2".equals(sortingMode)) {
            statisticsMap = new TreeMap<>(new MageComparator());
        } else {
            throw new IllegalArgumentException("Invalid sorting mode: " + sortingMode);
        }

        for (Mage mage : mageSet) {
            int descendantsCount = mage.countApprentices();
            statisticsMap.put(mage, descendantsCount);
        }
    }
    public void printStatistics() {
        System.out.println("\n Mage statictics: \n");
        for (Map.Entry<Mage, Integer> entry : statisticsMap.entrySet()) {
            System.out.println(entry.getKey().toString() + " - Descendants: " + entry.getValue());
        }
    }
}
