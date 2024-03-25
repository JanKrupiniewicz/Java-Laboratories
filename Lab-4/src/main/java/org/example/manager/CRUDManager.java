package org.example.manager;

import jakarta.persistence.EntityManager;
import org.example.util.JPAUtil;

import java.util.Scanner;

public abstract class CRUDManager {
    protected EntityManager entityManager;
    protected Scanner scanner;
    public CRUDManager() {
        entityManager = JPAUtil.getEntityManagerFactory().createEntityManager();
        scanner = new Scanner(System.in);
    }
    public abstract void getAll();
}
