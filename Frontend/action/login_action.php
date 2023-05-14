<?php    
    include_once("json_action.php");


    session_start();

    if ($_SERVER["REQUEST_METHOD"] == "POST") {
        if (!empty($_POST['email']) && !empty($_POST['ps'])) {
            $hash = hash('sha256', $_POST['ps']);
            $data = array('email' => $_POST['email'] ,'password' => $hash);
            $response = send_post_request('loginAuth', $data);

            if ($response != "Username or Password not right"){
                
                $verificationCode = uniqid();
                $_SESSION['timecreated'] = time();
                $_SESSION['submitted_time'] = "";
                $_SESSION['verificationCode'] = $verificationCode;
                $_SESSION['email'] = $_POST['email'];


                $data = send_post_request("LoginAttempt", array('name' => 'non', 'password' => 'non', 'email' => $_POST['email']));
                echo stripslashes($data);
                $data2 = json_decode(trim(stripslashes($data), '"'), true); 
                if ($data2 === null) {
                    echo "Error decoding JSON: " . json_last_error_msg();
                } else {
                    
                }
                $_SESSION['tempName'] = $data2["Name"];
                var_dump($data2);
                if (send_get_request("sendEmail?" . "email=" . $_POST['email'] . "&uniqueKey=" . $verificationCode) == "Ok") {
                    header("Location: ../index2fa.php");
                } 
                else {
                    $_SESSION['error'] = "chyba v posilaní emailu, zkuste později";
                    header("Location: ../login.php");
                }
                

                
                
                
            }
            else {
                $_SESSION['error'] = "Špatné jméno nebo heslo";
                header("Location: ../login.php");
                exit();
            }
        }
        else if (!empty($_POST['code']))
        {
            // 2fa ověření


        }
        else {
            echo "??"; // you are never supposed to see this!
            $_SESSION['error'] = "Prosím vyplňte všechna pole";
            header("Location: ../login.php");
            exit();
        }

    }
    else {
        header("Location: ../index.php");
        session_abort();
        exit();
    }





?>
