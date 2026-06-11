<?php
require 'db_connect.php';

$studentId = $_POST['studentId'];
$labName = $_POST['labName']; // e.g., "lab1"
$score = $_POST['score'];
$total = $_POST['total'];

// Quick MVP mapping: Which quiz did they take?
$quizId = 1; 
if($labName == "lab2") $quizId = 2;

// Calculate percentage score
$percentage = ($score / $total) * 100;

// Insert the test result into the QuizAttempt table
$sql = "INSERT INTO QuizAttempt (quizId, studentId, startTime, endTime, status, getScore) 
        VALUES ($quizId, $studentId, NOW(), NOW(), 'completed', $percentage)";

if ($conn->query($sql) === TRUE) {
    echo "success";
} else {
    echo "Error: " . $sql . "<br>" . $conn->error;
}

$conn->close();
?>