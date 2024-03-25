package org.example.manager;

import jakarta.persistence.EntityTransaction;
import jakarta.persistence.Query;
import org.example.entity.Mage;
import org.example.entity.Tower;

import java.util.List;

public class TowerManager extends CRUDManager {
    public void insertTower(Tower tower) {
        EntityTransaction transaction = entityManager.getTransaction();
        transaction.begin();
        entityManager.persist(tower);
        entityManager.getTransaction().commit();
    }
    public void deleteTower(String towerName) {
        EntityTransaction transaction = entityManager.getTransaction();
        transaction.begin();

        Tower tower = entityManager.find(Tower.class, towerName);
        if (tower != null) {
            List<Mage> mages = tower.getMages();
            if (mages != null) {
                for (Mage mage : mages) {
                    mage.deleteTower();
                    entityManager.merge(mage);
                }
            }
            entityManager.remove(tower);
        }
        transaction.commit();
    }

    public List<Tower> getAllTowersWithHeightLessThan(int maxHeight) {
        Query query = entityManager.createQuery("SELECT t FROM Tower t WHERE t.height < :maxHeight", Tower.class);
        query.setParameter("maxHeight", maxHeight);
        return query.getResultList();
    }
    public Tower findTowerByName(String towerName) {
        return entityManager.find(Tower.class, towerName);
    }
    public void getAll() {
        Query query = entityManager.createQuery("SELECT t FROM Tower t", Tower.class);
        List<Tower> towersList = query.getResultList();

        if (towersList.isEmpty()) {
            System.out.println("No towers to show.");
            return;
        }

        int itr = 0;
        for (Tower tower : towersList) {
            System.out.println("\nTOWER " + (++itr) + ": ");
            System.out.println("Name: " + tower.getName());
            System.out.println("Height: " + tower.getHeight());
            List<Mage> mages = tower.getMages();
            if (!mages.isEmpty()) {
                System.out.println("Mages: ");
                for (Mage mage : mages) {
                    System.out.println("   Mage: " + mage.getName() + ", Level: " + mage.getLevel());
                }
            } else {
                System.out.println("No mages for this tower.");
            }
        }
    }
}
