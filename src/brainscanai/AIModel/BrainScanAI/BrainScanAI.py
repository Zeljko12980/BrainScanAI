import base64
import cv2
import numpy as np
import tensorflow as tf
import grpc
from concurrent import futures
import brain_pb2
import brain_pb2_grpc


model = tf.keras.models.load_model("model/modelclassification97.h5")

categories = ['glioma', 'meningioma', 'pituitary', 'no']

def predict_image_from_base64(base64_str):
    
    image_bytes = base64.b64decode(base64_str)

  
    file_bytes = np.frombuffer(image_bytes, np.uint8)
    img = cv2.imdecode(file_bytes, cv2.IMREAD_COLOR)

    
    img = cv2.resize(img, (128, 128)) / 255.0
    img = np.expand_dims(img, axis=0)

  
    prediction = model.predict(img)
    class_index = np.argmax(prediction)
    confidence = float(np.max(prediction))

    return categories[class_index], confidence

class BrainTumorAnalyzerServicer(brain_pb2_grpc.BrainTumorAnalyzerServicer):
    def Predict(self, request, context):
        try:
            base64_image = request.image  
            tumor_type, confidence = predict_image_from_base64(base64_image)

            return brain_pb2.TumorPredictionResponse(
                tumor_type=tumor_type,
                confidence=confidence
            )
        except Exception as e:
            context.set_details(str(e))
            context.set_code(grpc.StatusCode.INTERNAL)
            return brain_pb2.TumorPredictionResponse()

def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    brain_pb2_grpc.add_BrainTumorAnalyzerServicer_to_server(
        BrainTumorAnalyzerServicer(), server
    )
    server.add_insecure_port('192.168.0.13:50051')

    print("✅ gRPC server pokrenut na portu 50051...")
    server.start()
    server.wait_for_termination()

if __name__ == "__main__":
    serve()
