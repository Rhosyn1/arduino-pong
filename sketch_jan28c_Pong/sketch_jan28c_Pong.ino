const int ledPin = 13;

int incomingByte;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(ledPin, OUTPUT);
}

void loop() {
  // put your main code here, to run repeatedly:
  if(Serial.available() > 0){
    incomingByte = Serial.read();
    if(incomingByte == 'H'){
      digitalWrite(ledPin, HIGH);
    }
    if(incomingByte == 'L'){
      digitalWrite(ledPin, LOW);
    }
    if(incomingByte == 'I'){
      Serial.print(analogRead(A0));
      Serial.print("-");
      Serial.println(analogRead(A1));
    }
  }
}
