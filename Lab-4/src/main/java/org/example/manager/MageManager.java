package org.example.manager;

import jakarta.persistence.EntityTransaction;
import jakarta.persistence.Query;
import org.example.entity.Mage;
import org.example.entity.Tower;

import java.util.List;

public class MageManager extends CRUDManager {
    public void insertMage(Mage mage) {
        EntityTransaction transaction = entityManager.getTransaction();
        transaction.begin();
        entityManager.persist(mage);
        entityManager.getTransaction().commit();
    }
    public List<Mage> getAllMagesWithLevelGreaterThanInTower(Tower tower, int minimalLevel) {
        Query query = entityManager.createQuery("SELECT m FROM Mage m WHERE m.tower = :tower AND m.level > :minimalLevel", Mage.class);
        query.setParameter("tower", tower);
        query.setParameter("minimalLevel", minimalLevel);
        return query.getResultList();
    }
    public List<Mage> getAllMagesWithLevelGreaterThan(int minLevel) {
        Query query = entityManager.createQuery("SELECT m FROM Mage m WHERE m.level > :minLevel", Mage.class);
        query.setParameter("minLevel", minLevel);
        return query.getResultList();
    }
    public void deleteMage(String mageName) {
        EntityTransaction transaction = entityManager.getTransaction();
        transaction.begin();
        Mage mage = entityManager.find(Mage.class, mageName);
        if (mage != null) {
            Tower mageTower = mage.getTower();
            if (mageTower != null) {
                mageTower.deleteMage(mage);
            }
            entityManager.remove(mage);
        }
        transaction.commit();
    }
    public Mage findMageByName(String mageName) {
        return entityManager.find(Mage.class, mageName);
    }
    public void getAll() {
        Query query = entityManager.createQuery("SELECT m FROM Mage m", Mage.class);
        List<Mage> magesList = query.getResultList();

        if (magesList.isEmpty()) {
            System.out.println("No mages to show.");
            return;
        }

        int itr = 0;
        for (Mage mage : magesList) {
            System.out.println("\nMAGE " + (++itr) + ": ");
            System.out.println("Name: " + mage.getName());
            System.out.println("Level: " + mage.getLevel());
            Tower mageTower = mage.getTower();
            if (mageTower != null) {
                System.out.println("Tower: name - " + mageTower.getName() + ", height - " + mageTower.getHeight());
            } else {
                System.out.println("Mage doesn't have a tower.");
            }
        }
    }
}
