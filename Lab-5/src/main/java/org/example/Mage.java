package org.example;
import lombok.*;
import java.util.Objects;

@NoArgsConstructor
public class Mage {
    @Getter @Setter
    private String name;
    @Getter @Setter
    private int level;
    public Mage(String name, int level) {
        this.name = name;
        this.level = level;
    }

    @Override
    public String toString() {
        return "Mage: { name: " + name + ", level: " + level + " }";
    }
    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null) return false;
        if (this.getClass() != o.getClass()) return false;
        Mage other = (Mage) o;
        return Objects.equals(name, other.name) &&
                (level == other.level);
    }
}

