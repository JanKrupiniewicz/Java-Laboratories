package org.example;
import java.util.Comparator;

public class MageComparator implements Comparator<Mage> {
    @Override
    public int compare(Mage mage1, Mage mage2) {
        return Integer.compare(mage1.getLevel(), mage2.getLevel());
    }
}
