package org.example;

import org.example.Mage;
import java.util.*;

public class MageSetFactory {
    private final Set<Mage> mageSet;
    private final Integer sortingMode;
    public MageSetFactory(String sortingMode) {
        if("noSotring".equalsIgnoreCase(sortingMode) || "0".equals(sortingMode)) {
            // No sorting
            this.sortingMode = 0;
            mageSet =  new HashSet<>();
        } else if ("naturalOrder".equalsIgnoreCase(sortingMode) || "1".equals(sortingMode)) {
            // Sorting Naturally
            this.sortingMode = 1;
            mageSet =  new TreeSet<>();
        } else if ("alternativeOrder".equalsIgnoreCase(sortingMode) || "2".equals(sortingMode)) {
            // Sorting by Level
            this.sortingMode = 2;
            mageSet =  new TreeSet<>(new MageComparator());
        } else {
            throw new IllegalArgumentException("Invalid sorting mode: " + sortingMode);
        }
    }
    public void generateMags() {
        Mage m1 = new Mage("Adam", 10, 50);
        Mage m2 = new Mage("Kacper", 11, 100);
        Mage m3 = new Mage("Kamil", 12, 100);
        Mage m4 = new Mage("Artur", 5, 150);
        Mage m5 = new Mage("Marcin", 6, 100);
        Mage m6 = new Mage("Hubert", 7, 50);
        Mage m7 = new Mage("Zofia", 3, 50);
        Mage m8 = new Mage("Kasia", 10, 150);

        Mage magRec1 = new Mage("Rekursywny Mag I", 15, 200);
        magRec1.addApprentice(m1);
        magRec1.addApprentice(m2);
        magRec1.addApprentice(m3);
        Mage magRec2 = new Mage("Rekursywny Mag II", 4, 150);
        magRec2.addApprentice(magRec1);
        magRec2.addApprentice(m7);
        magRec2.addApprentice(m8);
        Mage magRec3 = new Mage("Rekursywny Mag III", 20, 300);
        magRec3.addApprentice(magRec1);
        magRec3.addApprentice(magRec2);
        magRec3.addApprentice(m4);

        mageSet.add(m1);
        mageSet.add(m2);
        mageSet.add(m3);
        mageSet.add(m4);
        mageSet.add(m5);
        mageSet.add(m6);
        mageSet.add(m7);
        mageSet.add(m8);
        mageSet.add(magRec1);
        mageSet.add(magRec2);
        mageSet.add(magRec3);
    }
    public void addAllMags(List<Mage> magList) {
        mageSet.addAll(magList);
    }
    public void addMag(Mage mage) {
        mageSet.add(mage);
    }
    public Set<Mage> getMags() {
        return mageSet;
    }
    public void printMageSet() {
        System.out.println("\n Mage recursive output: \n");
        Set<Mage> visitedMages = new HashSet<>();

        // Sort the mageSet based on the number of apprentices in descending order
        List<Mage> sortedMages = new ArrayList<>(mageSet);
        sortedMages.sort(Comparator.comparingInt(mage -> -mage.countApprentices()));

        for (Mage mage : sortedMages) {
            printMageRecursive(mage, 1, visitedMages);
        }
    }

    private void printMageRecursive(Mage mage, int recLevel, Set<Mage> visitedMages) {
        if (!visitedMages.contains(mage)) {
            visitedMages.add(mage);

            for (int i = 0; i < recLevel; i++) {
                System.out.print("-");
            }
            System.out.println(mage.toString());

            List<Mage> sortedApprentices = new ArrayList<>(mage.getApprentices());
            sortedApprentices.sort(Comparator.comparingInt(app -> -app.countApprentices()));

            for (Mage mageApp : sortedApprentices) {
                printMageRecursive(mageApp, recLevel + 1, visitedMages);
            }
        }
    }
}
