package org.example;

import org.apache.commons.lang3.tuple.Pair;

import javax.imageio.ImageIO;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.IOException;
import java.nio.file.*;
import java.util.List;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class Main {
    public static void main(String[] args) {
        if (args.length == 0) {
            System.out.println("No arguments provided");
            return;
        }

        String inputDir = args[0];
        String outputDir = args[1];
        int threadPoolSize = Integer.parseInt(args[2]);

//        String inputDir = "C:\\Users\\jkrup\\OneDrive\\Dokumenty\\Semestr IV 2023_2024\\Platformy technologiczne\\Laboratorium\\Lab6-1\\src\\main\\resources\\photos";
//        String outputDir = "C:\\Users\\jkrup\\OneDrive\\Dokumenty\\Semestr IV 2023_2024\\Platformy technologiczne\\Laboratorium\\Lab6-1\\src\\main\\resources\\output";
//        int threadPoolSize = 4;

        Path inputDirPath = Paths.get(inputDir);
        Path outputDirPath = Paths.get(outputDir);

        try {
            Files.createDirectories(outputDirPath);
        } catch (IOException e) {
            e.printStackTrace();
        }

        long time = System.currentTimeMillis();

        ExecutorService executor = Executors.newFixedThreadPool(threadPoolSize);

        try (Stream<Path> stream = Files.list(inputDirPath)) {
            List<Path> files = stream.collect(Collectors.toList());

            for (Path path : files) {
                executor.submit(() -> processImage(path, outputDirPath));
            }
        } catch (IOException e) {
            e.printStackTrace();
        }

        executor.shutdown();
        System.out.println("Time : " + (System.currentTimeMillis() - time));
    }

    private static void processImage(Path inputImagePath, Path outputDirPath) {
        String fileName = inputImagePath.getFileName().toString();
        try {
            BufferedImage image = ImageIO.read(inputImagePath.toFile());
            BufferedImage newImage = new BufferedImage(
                    image.getWidth(),
                    image.getHeight(),
                    image.getType()
            );

            for (int i = 0; i < image.getWidth(); i++) {
                for (int j = 0; j < image.getHeight(); j++) {
                    int rgb = image.getRGB(i, j);
                    Color color = new Color(rgb);
                    int red = color.getRed();
                    int blue = color.getBlue();
                    int green = color.getGreen();
                    Color newColor = new Color(red, blue, green);
                    int newRgb = newColor.getRGB();
                    newImage.setRGB(i, j, newRgb);
                }
            }

            Path outputPath = outputDirPath.resolve(fileName);
            ImageIO.write(newImage, "png", outputPath.toFile());
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}