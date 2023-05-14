<?php

    function send_post_request($url, $data) {
        // Encode the data as JSON
        $data_string = json_encode($data);
    
        // Set the curl options
        $ch = curl_init("https://localhost:32780/" . $url);
        curl_setopt($ch, CURLOPT_POST, true);
        curl_setopt($ch, CURLOPT_POSTFIELDS, $data_string);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
        curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false,);
        curl_setopt($ch, CURLOPT_HTTPHEADER, array('Content-Type: application/json')
        );
    
        // Send the request and receive the response
        $response = curl_exec($ch);
        
        if ($response === false) {
            $error = curl_error($ch);
            curl_close($ch);
            return array('success' => false, 'error' => $error);
        }


        // Close the curl session
        curl_close($ch);
    
        // Return the response
        return $response;
    }

    function send_get_request($url) {
        $url = 'https://localhost:32780/' . $url;

        $options = array(
            'http' => array(
                'method' => 'GET'
            ),
            'ssl' => array(
                'verify_peer' => false,
                'verify_peer_name' => false
            )
        );

        $response = file_get_contents($url, false, stream_context_create($options));
        // prichazi pouze ok!
        //$data = json_decode($response, true); 
        return $response;
    }




?>