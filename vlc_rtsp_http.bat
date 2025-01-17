:: The following line is neccessary if you need an ability to restart the streams with this batch file
:: Kill all existing streams (the command actually suspends ALL the vlc processes):
taskkill /f /im "vlc.exe"

:: Run two instances of VLC. These would transcode MP4 rtsp-stream to Motion JPEG http-stream:
"C:\Program Files (x86)\VideoLAN\VLC\vlc.exe" -I dummy -vvv rtsp://192.168.10.51 --sout #transcode{vcodec=MJPG,venc=ffmpeg{strict=1},fps=10,width=640,height=360}:standard{access=http{mime=multipart/x-mixed-replace;boundary=--7b3cc56e5f51db803f790dad720ed50a},mux=mpjpeg,dst=:9911/}
:: -vvv -Idummy rtsp://192.168.10.51 --sout #transcode{vcodec=MJPG,venc=ffmpeg{strict=1},fps=10,width=640,height=360}:standard{access=http{mime=multipart/x-mixed-replace;boundary=--7b3cc56e5f51db803f790dad720ed50a},mux=mpjpeg,dst=:9911/}

::start "C:\Program Files (x86)\VideoLAN\VLC\vlc.exe -vvv -Idummy rtsp://192.168.10.51 --sout #transcode{vcodec=MJPG,venc=ffmpeg{strict=1},fps=10,width=640,height=360}:standard{access=http{mime=multipart/x-mixed-replace;boundary=--7b3cc56e5f51db803f790dad720ed50a},mux=mpjpeg,dst=:9912/}"

:: In order to execute VLC with `vlc` as in exapmle above, you have to add corresponding value to the PATH variable.
:: Otherwise you have to use full path e.g `"C:\Program Files\VLC\vlc.exe"`.

:: Such rtsp-stream uri as rtsp://login:password@127.0.0.1/streaming/channels/2/preview is specific for Hikvision cameras.

:: Access the http-streams like that:
:: For the first camera: http://127.0.0.1:9911 
:: For the the second camera: http://127.0.0.1:9912 
:: For external access change `127.0.0.1` to the address of the server (or PC) you are running VLC on.
:: Your server's Ports you are using for streaming (9911 and 9912 from current example) must be open for external access.