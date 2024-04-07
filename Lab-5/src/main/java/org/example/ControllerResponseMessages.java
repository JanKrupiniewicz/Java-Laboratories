package org.example;

public enum ControllerResponseMessages {
    DONE {
        @Override
        public String toString() {
            return "done";
        }
    },
    NOT_FOUND{
        @Override
        public String toString() {
            return "not found";
        }
    },
    BAD_REQUEST {
        @Override
        public String toString() {
            return "bad request";
        }
    }
}
