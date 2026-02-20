<?php
require 'db_connect.php';

$email = $_POST['email'];
$password = $_POST['password'];
$name = $_POST['name'];

// 1. Insert into User table
$sql_user = "INSERT INTO User (email, passwordHash, name, userType) VALUES ('$email', '$password', '$name', 'student')";

if ($conn->query($sql_user) === TRUE) {
    $last_id = $conn->insert_id;
    
    // 2. Insert into Student table (Linking userId)
    $sql_student = "INSERT INTO Student (userId, enrollmentDate) VALUES ($last_id, NOW())";
    
    if ($conn->query($sql_student) === TRUE) {
        echo "success";
    } else {
        echo "Error student: " . $conn->error;
    }
} else {
    echo "Error user: " . $conn->error;
}
$conn->close();
?>