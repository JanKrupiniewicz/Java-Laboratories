package org.example;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

import static org.junit.jupiter.api.Assertions.*;

class MageRepositoryTest {
    private MageRepository mageRepository;

    @BeforeEach
    void setUp() {
        mageRepository = new MageRepository();
        List<Mage> mageList = new ArrayList<>();
        mageList.add(new Mage("Merlin", 10));
        mageList.add(new Mage("Gandalf", 15));
        mageList.add(new Mage("Dumbledore", 20));
        mageRepository.setCollection(mageList);
    }

    @Test
    public void testDelete_notFound() {
        assertThrows(IllegalArgumentException.class, () -> mageRepository.delete("Gandalf2"));
    }

    @Test
    public void testFind_notFound() {
        Optional<Mage> mage = mageRepository.find("Sauron");
        assertFalse(mage.isPresent());
    }

    @Test
    public void testFind_returnCorrectObject() {
        Optional<Mage> mage = mageRepository.find("Gandalf");
        assertTrue(mage.isPresent());
        assertEquals("Gandalf", mage.get().getName());
        assertEquals(15, mage.get().getLevel());
    }

    @Test
    public void testSaveObject_alreadyPresent() {
        assertThrows(IllegalArgumentException.class, () -> mageRepository.save(new Mage("Gandalf", 25)));
    }
}