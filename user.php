<?php

if (!isset($_GET["id"]) || ($_GET["id"] == "")) {
    readfile("http://img.shields.io/badge/404-user%20unknown-lightgrey.png");
    die();
}

if ($_GET["id"] != strtolower($_GET["id"])) {
    header("Location: user.php?id=" . strtolower($_GET["id"]));
}

$authed_users = explode("\n", file_get_contents("user_cache.txt")); // username to user id cache

foreach ($authed_users as &$user) {
    $user_and_id = explode(" ", $user);
    
    $id       = $user_and_id[0];
    $username = $user_and_id[1];
    
    if ($username == $_GET["id"]) {
        header('Content-Type:image/png');
        
        if (exec("sh ../../shield_generator.sh " . escapeshellarg($id) . " 2>&1") == "online") {
            readfile($username . "/online.png");
        } else {
            readfile($username . "/offline.png");
        }
        die();
    }
}

file_get_contents("add_user.php?id=" . strtolower($_GET["id"]));
sleep(3);

echo "Your shield has been generated. Please refresh.";

?>
