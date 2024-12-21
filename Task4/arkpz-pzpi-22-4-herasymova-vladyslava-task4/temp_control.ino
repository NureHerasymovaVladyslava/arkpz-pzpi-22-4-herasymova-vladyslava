#include <WiFi.h>
#include <HTTPClient.h>
#include <NetworkClientSecure.h>
#include <EEPROM.h>

#define LED_PIN 18
#define BUTTON_REGISTER_PIN 16
#define BUTTON_TURN_ON_OFF_PIN 17
#define WIFI_SSID "Wokwi-GUEST"
#define WIFI_PASSWORD ""
#define SENSOR_TYPE "1"

#define EEPROM_SIZE 5
#define DEVICE_ID_ADDR 0
#define WORKING_STATE_ADDR 4

int deviceId = -1;
bool isWorking = false;
int delaySecs = 0;

String serverUrl = "https://tdvjj1ts-7089.euw.devtunnels.ms/api";
String createControlUrl = "/Control/create";
String checkStateUrl = "/Control/check-state/";

int readDeviceIdFromEEPROM();
bool readWorkingStateFromEEPROM();
void registerDevice();
void toggleWorkingState();
void sensorFunctionality();

void setup() {
  Serial.begin(115200);

  EEPROM.begin(EEPROM_SIZE);

  deviceId = readDeviceIdFromEEPROM();
  if (deviceId != -1) {
    Serial.print("Device ID loaded from EEPROM: ");
    Serial.println(deviceId);
  } else {
    Serial.println("No device ID found. Please press the button to register.");
  }

  isWorking = readWorkingStateFromEEPROM();

  pinMode(BUTTON_REGISTER_PIN, INPUT_PULLUP);
  pinMode(BUTTON_TURN_ON_OFF_PIN, INPUT_PULLUP);
  pinMode(LED_PIN, OUTPUT);

  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);

  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println("Connecting to WiFi...");
  }
  Serial.println("Connected to WiFi");
}

void loop() {
  if (digitalRead(BUTTON_REGISTER_PIN) == LOW) {
    Serial.println("Button Pressed! Registering device...");
    registerDevice();
    delaySecs++;
    delay(100);
  }
  if (digitalRead(BUTTON_TURN_ON_OFF_PIN) == LOW) {
    Serial.println("Button Pressed! Changing device state...");
    toggleWorkingState();
    delaySecs++;
    delay(100);
  }

  if (deviceId != -1 && isWorking && delaySecs >= 100) {
    delaySecs = 0;
    sensorFunctionality();
  }

  delaySecs++;
  delay(100);
  Serial.print("Delay secs: ");
  Serial.println(delaySecs);
}

void sensorFunctionality() {
  if (WiFi.status() == WL_CONNECTED) {
    HTTPClient http;
    http.begin(serverUrl + checkStateUrl + String(deviceId));
    int httpResponseCode = http.GET();

    if (httpResponseCode == 200) {
      String response = http.getString();
      bool turnedOn = response == "true";
      Serial.print("Device is On: ");
      Serial.println(turnedOn);

      if (turnedOn) {
        digitalWrite(LED_PIN, HIGH);
      }
      else {
        digitalWrite(LED_PIN, LOW);
      }
    } else {
      Serial.print("Failed to register device state, HTTP code: ");
      Serial.println(httpResponseCode);
    }

    http.end();
  } else {
    Serial.println("WiFi not connected");
  }
}

void registerDevice() {
  if (deviceId != -1) {
    Serial.println("Device is already registered.");
    Serial.print("Device ID: ");
    Serial.println(deviceId);
    return;
  }
  if (WiFi.status() == WL_CONNECTED) {
    HTTPClient http;
    http.begin(serverUrl + createControlUrl);
    http.addHeader("Content-Type", "application/json");

    int httpResponseCode = http.POST(SENSOR_TYPE);

    if (httpResponseCode == 200) {
      String response = http.getString();
      deviceId = response.toInt();

      for (int i = 0; i < EEPROM_SIZE; i++) {
        EEPROM.write(DEVICE_ID_ADDR + i, (deviceId >> (i * 8)) & 0xFF);
      }
      EEPROM.commit();
      Serial.println("Device ID stored in EEPROM.");

      Serial.println("Device registered successfully!");
      Serial.print("Device ID: ");
      Serial.println(deviceId);
    } else {
      Serial.print("Failed to register device, HTTP code: ");
      Serial.println(httpResponseCode);
    }
    http.end();
  } else {
    Serial.println("WiFi not connected!");
  }
}

int readDeviceIdFromEEPROM() {
  int id = 0;
  for (int i = 0; i < EEPROM_SIZE; i++) {
    id |= (EEPROM.read(DEVICE_ID_ADDR + i) << (i * 8));
  }

  if (id == 0xFFFFFFFF || id == 0) {
    return -1;
  }
  return id;
}

void toggleWorkingState() {
  if (deviceId == -1) {
    Serial.println("Device is not registered.");
    Serial.println("Working state change canceled");
    return;
  }
  isWorking = !isWorking;
  if (!isWorking) {
    digitalWrite(LED_PIN, LOW);
  }
  EEPROM.write(WORKING_STATE_ADDR, isWorking ? 1 : 0);
  EEPROM.commit();
  Serial.println("Working state stored in EEPROM.");

  Serial.print("Device working state changed to: ");
  Serial.println(isWorking ? "On" : "Off");
}

bool readWorkingStateFromEEPROM() {
  return EEPROM.read(WORKING_STATE_ADDR) == 1;
}
