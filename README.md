<p align="center">
  <img width="260" alt="캡처" src="https://s3.us-west-2.amazonaws.com/secure.notion-static.com/5706aeda-e608-45c2-8da7-b56d83f9ff5e/instagram_profile_image.png?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAT73L2G45O3KS52Y5%2F20210605%2Fus-west-2%2Fs3%2Faws4_request&X-Amz-Date=20210605T165102Z&X-Amz-Expires=86400&X-Amz-Signature=e75a85dc498e10e02b80a4c2b9cc96ca256146b75231c6450a9092b0c685ae0b&X-Amz-SignedHeaders=host&response-content-disposition=filename%20%3D%22instagram_profile_image.png%22">
</p>
<p align="center">
  <b>
    AR기반 온라인 가구 오픈마켓 서비스
  </b>
</p>
<p align="center">
  <b>
    AR-based online furniture open market service
  </b>
</p>

<br>

## **Table of Content**
---

1. 기능
2. 사용 기술
2. 실행 화면
3. 개발 환경

<br>

## 1. 기능
---
### Place mannequin ; 마네킹 배치

- Mannequin Size Adjustment - 마네킹 크기 조절
- Mannequin toggle function - 마네킹 토글 기능
- Change mannequin position - 마네킹 위치 변경
<!-- slide -->

### furniture layout : 가구 배치

- Move Furniture Location - 가구 위치 이동
- Furniture Rotation Function - 가구 회전 기능
- Furniture Color Temperature Function - 가구 색 온도 기능


<br>

## 2. 사용 기술
---
### AR 기능 

안가구의 AR 기능에는 안드로이드 AR Core와 IOS의 AR Kit를 혼용하여 사용하는 AR 구현 Tool 인 AR Foundation을 사용합니다. AR Foundation은 COM 프로세스를 통하여 스마트폰의 POSE(Position and Orientation)을 계산해내고, 증강 현실을 위한 정보 및 객체를 화면에 표시해주는 알고리즘을 내부적으로 가지고 있습니다. 이 알고리즘은 "앵커" 라는 실세계에서의 가상의 마커 역할을 생성하는데, 기준이 되는 좌표계를 추출하기 위해 바닥 인식이 필요하다.

### **AR 바닥 인식 개발**

안가구 AR 환경에 3D model을 배치하기 위한 바닥 영역 인식 기능은 AR Foundation의 AR plane detection과 AR Raycast를 이용하여 구현해주었습니다. AR plane detection은 COM 프로세스를 통하여 카메라 영상으로 부터 추출된 특징점들로 클러스터를 구성합니다. 수평 또는 수직의 평면을 감지해 주어 앞서 말한 좌표계를 구성하되, 초기 상태에서 인식되는 평면의 크기가 작아, 오히려 바닥 인식의 정확도가 낮아지는 문제가 있었습니다. 따라서 AR Raycast 과정에서 Threshold로 특정 크기 이하의 평면을 필터링을 해주어 바닥 인식의 정확도를 높여주었습니다.

<br>
<p align="center">
  <img alt="capture" src="https://user-images.githubusercontent.com/55892515/120933273-513b4e00-c734-11eb-97c5-c560b3d258dc.gif">
</p>

<br>

## 3. 실행 화면
---
<br>


<p align="center">
  <img width="260" alt="캡처" src="https://user-images.githubusercontent.com/55892515/120900799-e590ac80-c671-11eb-8b41-69af674c3a4c.gif">
  <img width="260" alt="캡처" src="https://user-images.githubusercontent.com/55892515/120900781-cabe3800-c671-11eb-9e1f-5d5fd4a793ef.gif">
</p>


## 4. 개발 환경

---
```bash
 - Unity : 2020.3.6.f1
 - AR Foundation : Version 4.1.7
 - Unity UI : Version 1.0.0
 - ARCore XR Plugin : Version 1.0.0
 ```
