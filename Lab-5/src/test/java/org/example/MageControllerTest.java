package org.example;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.MockitoAnnotations;

import java.util.Optional;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class MageControllerTest {
    @Mock
    private MageRepository mockedMageRepository;
    private MageController mageController;

    @BeforeEach
    void setUp() {
        MockitoAnnotations.openMocks(this);
        mageController = new MageController(mockedMageRepository);
    }

    @Test
    public void testDelete_deleted() {
        doNothing().when(mockedMageRepository).delete("Gandalf");
        String result = mageController.delete("Gandalf");
        assertEquals(ControllerResponseMessages.DONE.toString(), result);
        verify(mockedMageRepository, times(1)).delete("Gandalf");
    }


    @Test
    public void testDelete_notFound() {
        doThrow(new IllegalArgumentException()).when(mockedMageRepository).delete("Sauron");
        String result = mageController.delete("Sauron");
        assertEquals(ControllerResponseMessages.NOT_FOUND.toString(), result);
        verify(mockedMageRepository, times(1)).delete("Sauron");
    }

    @Test
    public void testFind_notFound() {
        when(mockedMageRepository.find("Sauron")).thenReturn(Optional.empty());
        assertEquals(ControllerResponseMessages.NOT_FOUND.toString(), mageController.find("Sauron"));
    }

    @Test
    public void testFind_Found() {
        Mage gandalf = new Mage("Gandalf", 15);
        when(mockedMageRepository.find("Gandalf")).thenReturn(Optional.of(gandalf));
        assertEquals(gandalf.toString(), mageController.find("Gandalf"));
        verify(mockedMageRepository, times(1)).find("Gandalf");
    }

    @Test
    public void testSaveObject_alreadyPresent() {
        doThrow(new IllegalArgumentException()).when(mockedMageRepository).save(new Mage("Gandalf", 15));
        assertEquals(ControllerResponseMessages.BAD_REQUEST.toString(), mageController.save("Gandalf", "15"));
    }

    @Test
    public void testSaveObject_saveObjectCorrectly() {
        when(mockedMageRepository.find("Sauron")).thenReturn(Optional.empty());
        assertEquals(ControllerResponseMessages.DONE.toString(), mageController.save("Sauron", "20"));
        verify(mockedMageRepository, times(1)).save(new Mage("Sauron", 20));
    }
}