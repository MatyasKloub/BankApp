<?php
    session_start();

    include("json_action.php");

    if ($_SERVER["REQUEST_METHOD"] == "POST") {
        if (!empty($_POST['name']) && !empty($_POST['email']) && !empty($_POST['ps']) && !empty($_POST['psa']))
        {
            if ($_POST['ps'] != $_POST['psa']) {
                $_SESSION['error'] = "Hesla se neshodují";
                header("Location: ../register.php");
                exit();
            }
            else {
                $hash = hash('sha256', $_POST['ps']);
                $data = array('name' => $_POST['name'], 'password' => $hash, 'email' => $_POST['email']);
                $response = send_post_request('registerAttempt', $data);
                // tady je třeba ošetřit co se může stat když server neni dostupny
                if ($response != "Email already exists") {
                    $_POST['error'] = 'Registrace proběhla úspěšně, přihlašte se prosím.';
                    header("Location: ../login.php");
                }
                else {
                    $_SESSION['error'] = "Účet s tímto emailem již existuje";
                    header("Location: ../register.php");
                }
            }
        }
        else 
        {
            header("Location: ../register.php");
            exit();
        }
    }
    else {
        header("Location: https://www.seznam.cz/");   
        exit(); 
    }

?>