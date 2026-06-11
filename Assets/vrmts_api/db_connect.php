<?php
$servername = "localhost";
$username = "root";      // Default XAMPP user
$password = "";          // Default XAMPP password is blank
$dbname = "vrmts";       // Matches your database name in the screenshot

$conn = new mysqli($servername, $username, $password, $dbname);
if ($conn->connect_error) { die("Connection failed: " . $conn->connect_error); }
?>