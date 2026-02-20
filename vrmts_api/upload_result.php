<?php
require 'db_connect.php';

$studentId = $_POST['studentId'];
$labName = $_POST['labName']; // Expecting "lab1", "lab2", etc.
$score = $_POST['score'];
$total = $_POST['total'];

// 1. Quick logic to find a quizId based on LabName (You might need to adjust this manually in DB)
// For MVP, we will assume:
// lab1 = quizId 1
// lab2 = quizId 2
$quizId = 1; 
if($labName == "lab2") $quizId = 2;

// Calculate percentage score
$percentage = ($score / $total) * 100;

// 2. Insert into QuizAttempt
$sql = "INSERT INTO QuizAttempt (quizId, studentId, startTime, endTime, status, getScore) 
        VALUES ($quizId, $studentId, NOW(), NOW(), 'completed', $percentage)";

if ($conn->query($sql) === TRUE) {
    echo "success";
} else {
    echo "Error: " . $sql . "<br>" . $conn->error;
}

$conn->close();
?>