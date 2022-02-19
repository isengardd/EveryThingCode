:: 如果配置了eyusdk，将.gradle文件拷贝到游戏工程目录
echo %cfgname%
set eyuGradle="..\..\Assets\Plugins\Android\mainTemplate.gradle_eyu"
for /F "tokens=2 delims=@ " %%a in ("%cfgname%") do (
    find /i /c "AD_LAB_EYU" ..\..\Assets\Config\%%a.json >NUL
    if not errorlevel 1 (
        if exist %eyuGradle% (
            rename %eyuGradle% mainTemplate.gradle
            rename %eyuGradle%.meta mainTemplate.gradle.meta
        ) else (
            echo "file not found: %eyuGradle%"
            exit 1
        )
    ) else (
        echo "not found eyu config"
    )
)