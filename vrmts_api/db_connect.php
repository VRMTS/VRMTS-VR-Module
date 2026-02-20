<?php
$servername = "localhost";
$username = "root";      // Default XAMPP user
$password = "";          // Default XAMPP password is empty
$dbname = "vrmts";       // Your database name

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}
?>