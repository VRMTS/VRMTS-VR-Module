<?php
require 'db_connect.php';

$email = $_POST['email'];
$password = $_POST['password']; // In production, use password_verify()

// Check User table
$sql = "SELECT u.userId, u.name, s.studentId 
        FROM User u 
        JOIN Student s ON u.userId = s.userId 
        WHERE u.email = '$email' AND u.passwordHash = '$password'";

$result = $conn->query($sql);

if ($result->num_rows > 0) {
    $row = $result->fetch_assoc();
    // Return a JSON object that Unity can parse into SimpleUser
    echo json_encode(array(
        "status" => "success", 
        "userId" => $row['userId'],
        "studentId" => $row['studentId'],
        "name" => $row['name'],
        "email" => $email
    ));
} else {
    echo "failed";
}
$conn->close();
?>