package org.example.util;

import org.example.entity.Mage;
import org.example.entity.Tower;
import org.example.manager.MageManager;
import org.example.manager.TowerManager;

import java.util.List;
import java.util.Scanner;

public class ApplicationManager {
    private final MageManager mageManager;
    private final TowerManager towerManager;
    private final Scanner scanner;
    public ApplicationManager() {
        mageManager = new MageManager();
        towerManager = new TowerManager();
        scanner = new Scanner(System.in);
    }
    public void manageUserInput() {
        while(true) {
            System.out.println("Input [CM, CT, R, Z1, Z2, Z3, DM, DT, FM, FT, Q] to perform actions: ");
            String input = scanner.nextLine();

            if (input.equalsIgnoreCase("CM")) {
                System.out.println("Input Mage [name, level, tower_name]: ");
                String mageName = scanner.nextLine();
                int mageLvl = Integer.parseInt(scanner.nextLine());
                String towerName = scanner.nextLine();
                Tower tower = towerManager.findTowerByName(towerName);
                if(tower != null) {
                    mageManager.insertMage(new Mage(mageName, mageLvl, tower));
                } else {
                    System.out.println("Invalid Tower Name.");
                }
            } else if (input.equalsIgnoreCase("CT")) {
                System.out.println("Input Tower [name, height]: ");
                String towerName = scanner.nextLine();
                int towerHeight = Integer.parseInt(scanner.nextLine());
                towerManager.insertTower(new Tower(towerName, towerHeight));
            } else if (input.equalsIgnoreCase("R")) {
                System.out.println("\n -- MAGES -- ");
                mageManager.getAll();
                System.out.println("\n -- TOWERS -- ");
                towerManager.getAll();
            } else if (input.equalsIgnoreCase("DM")) {
                System.out.println("Input Mage Name: ");
                String mageName = scanner.nextLine();
                mageManager.deleteMage(mageName);
            } else if (input.equalsIgnoreCase("DT")) {
                System.out.println("Input Tower Name: ");
                String towerName = scanner.nextLine();
                towerManager.deleteTower(towerName);
            } else if (input.equalsIgnoreCase("FM")) {
                System.out.println("Input Mage Name: ");
                String mageName = scanner.nextLine();
                Mage mage = mageManager.findMageByName(mageName);
                if (mage != null) {
                    System.out.println("Mage found: " + mage.getName());
                } else {
                    System.out.println("Mage not found.");
                }
            } else if(input.equalsIgnoreCase("FT")) {
                System.out.println("Input Tower Name: ");
                String towerName = scanner.nextLine();
                Tower tower = towerManager.findTowerByName(towerName);
                if (tower != null) {
                    System.out.println("Tower found: " + tower.getName());
                } else {
                    System.out.println("Tower not found.");
                }
            } else if (input.equalsIgnoreCase("Z1")) {
                System.out.println("Input minimal Mage level: ");
                int minLevel = Integer.parseInt(scanner.nextLine());
                List<Mage> mages = mageManager.getAllMagesWithLevelGreaterThan(minLevel);
                if (!mages.isEmpty()) {
                    System.out.println("Mages with level greater than " + minLevel + ":");
                    for (Mage mage : mages) {
                        System.out.println("Mage: " + mage.getName() + ", Level: " + mage.getLevel());
                    }
                } else {
                    System.out.println("No mages with level greater than " + minLevel);
                }
            } else if (input.equalsIgnoreCase("Z2")) {
                System.out.println("Input maximal Tower height: ");
                int maxHeight = Integer.parseInt(scanner.nextLine());
                List<Tower> towers = towerManager.getAllTowersWithHeightLessThan(maxHeight);
                if (!towers.isEmpty()) {
                    System.out.println("Towers with height less than " + maxHeight + ":");
                    for (Tower tower : towers) {
                        System.out.println("Name: " + tower.getName() + ", height: " + tower.getHeight());
                    }
                } else {
                    System.out.println("No towers with height less than " + maxHeight);
                }
            } else if(input.equalsIgnoreCase("Z3")) {
                System.out.println("Input Tower Name: ");
                String towerName = scanner.nextLine();
                Tower tower = towerManager.findTowerByName(towerName);
                if (tower != null) {
                    System.out.println("Input minimal Mage level: ");
                    int minimalLevel = Integer.parseInt(scanner.nextLine());
                    List<Mage> mages = mageManager.getAllMagesWithLevelGreaterThanInTower(tower, minimalLevel);
                    if (!mages.isEmpty()) {
                        System.out.println("Mages with level greater " + minimalLevel + " than in tower " + towerName + ":");
                        for (Mage mage : mages) {
                            System.out.println("Mage: " + mage.getName() + ", Level: " + mage.getLevel());
                        }
                    } else {
                        System.out.println("No mages with level greater than in tower " + towerName);
                    }
                } else {
                    System.out.println("Tower not found.");
                }
            }
            else if (input.equalsIgnoreCase("Q")) {
                break;
            } else {
                System.out.println("Unknown command.");
            }
        }
    }
    public void createSampleData() {
        Tower tower1 = new Tower("Tower1", 100);
        Tower tower2 = new Tower("Tower2", 110);
        Tower tower3 = new Tower("Tower3", 150);
        Tower tower4 = new Tower("Tower4", 120);
        Tower tower5 = new Tower("Tower5", 140);
        Tower tower6 = new Tower("Tower6", 170);

        towerManager.insertTower(tower1);
        towerManager.insertTower(tower2);
        towerManager.insertTower(tower3);
        towerManager.insertTower(tower4);
        towerManager.insertTower(tower5);
        towerManager.insertTower(tower6);

        mageManager.insertMage(new Mage("Mage1", 10, tower1));
        mageManager.insertMage(new Mage("Mage2", 15, tower1));
        mageManager.insertMage(new Mage("Mage3", 12, tower2));
        mageManager.insertMage(new Mage("Mage4", 18, tower3));
        mageManager.insertMage(new Mage("Mage5", 20, tower4));
        mageManager.insertMage(new Mage("Mage6", 8, tower5));
    }
}
