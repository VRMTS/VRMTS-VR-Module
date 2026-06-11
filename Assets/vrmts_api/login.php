<?php
require 'db_connect.php';

$email = $_POST['email'];
$password = $_POST['password'];

// Query the User and Student tables
$sql = "SELECT u.userId, u.name, s.studentId 
        FROM user u 
        LEFT JOIN student s ON u.userId = s.userId 
        WHERE u.email = '$email' AND u.passwordHash = '$password'";

$result = $conn->query($sql);

if ($result->num_rows > 0) {
    $row = $result->fetch_assoc();
    // Send back JSON data to Unity
    echo json_encode(array(
        "status" => "success", 
        "userId" => $row['userId'],
        "studentId" => $row['studentId'] ? $row['studentId'] : 0, // Fallback if admin/teacher logs in
        "name" => $row['name'],
        "email" => $email
    ));
} else {
    echo "failed";
}
$conn->close();
?>