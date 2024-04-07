package org.example;

import lombok.*;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Objects;
import java.util.Optional;

public class MageRepository {
    @Getter @Setter
    private Collection<Mage> collection;

    public MageRepository() {
        collection = new ArrayList<>();
    }
    public Optional<Mage> find(String name) {
        for (Mage m : collection) {
            if (Objects.equals(m.getName(), name)) {
                return Optional.of(m);
            }
        }
        return Optional.empty();
    }
    public void delete(String name) {
        boolean removed =  collection.removeIf(m -> Objects.equals(m.getName(), name));
        if (!removed) {
           throw new IllegalArgumentException("Attempting to delete a non-existent object.");
        }
    }
    public void save(Mage mage) {
        Optional<Mage> existingMage = find(mage.getName());
        if (existingMage.isPresent()) {
            throw new IllegalArgumentException("Attempting to save an object whose primary key is already in the repository.");
        }
        collection.add(mage);
    }

}
