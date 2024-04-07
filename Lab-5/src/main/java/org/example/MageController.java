package org.example;

import java.util.Optional;

public class MageController {
    private final MageRepository repository;
    public MageController(MageRepository repository) {
        this.repository = repository;
    }
    public String find(String name) {
        Optional<Mage> mageOptional = repository.find(name);
        return mageOptional.map(Mage::toString).orElse(ControllerResponseMessages.NOT_FOUND.toString());
    }
    public String delete(String name) {
        try {
            repository.delete(name);
            return ControllerResponseMessages.DONE.toString();
        } catch (IllegalArgumentException e) {
            return ControllerResponseMessages.NOT_FOUND.toString();
        }
    }
    public String save(String name, String level) {
        try {
            repository.save(new Mage(name, Integer.parseInt(level)));
            return ControllerResponseMessages.DONE.toString();
        } catch (IllegalArgumentException e) {
            return ControllerResponseMessages.BAD_REQUEST.toString();
        }
    }
}
