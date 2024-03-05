package org.example;

import java.util.HashSet;
import java.util.Objects;
import java.util.Set;
import java.util.TreeSet;

public class Mage implements Comparable<Mage> {
    private final String name;
    private final int level;
    private final double power;
    private final Set<Mage> apprentices;

    public Mage(String name, int level, double power) {
        this.name = name;
        this.level = level;
        this.power = power;
        this.apprentices = new HashSet<>();
    }

    public void addApprentice(Mage mage) {
        apprentices.add(mage);
    }

    public Set<Mage> getApprentices() {
        return apprentices;
    }
    public int countApprentices() {
        int numberApp = 0;
        for (Mage mageApp : apprentices) {
            numberApp += countApprenticesRec(mageApp);
        }
        return numberApp;
    }
    private int  countApprenticesRec(Mage mage) {
        int numberApp = 1;
        if (mage.apprentices.size() > 0) {
            for (Mage mageApp : mage.getApprentices()) {
                numberApp += countApprenticesRec(mageApp);
            }
        }
        return numberApp;
    }

    public int getLevel() {
        return level;
    }
    public double getPower() {
        return power;
    }
    public String getName() {
        return name;
    }

    @Override
    public int hashCode() {
        int hash = 7;
        hash = 31 * hash + level;
        hash = 31 * hash + (int) power;
        hash = 31 * hash + (name == null ? 0 : name.hashCode());
        return hash;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null) return false;
        if (this.getClass() != o.getClass()) return false;
        Mage other = (Mage) o;
        return Objects.equals(name, other.name) &&
                (level == other.level) &&
                power == other.power &&
                apprentices.equals(other.apprentices);
    }
    @Override
    public int compareTo(Mage other) {
        return this.name.compareTo(other.name);
    }

    @Override
    public String toString() {
        return "Mage: { name: " + name + ", level: " + level + ", power: " + power + " }";
    }
}
