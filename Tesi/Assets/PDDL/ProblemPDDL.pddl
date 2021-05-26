(define 
(problem S_Example_P)
(:domain S_Example)
(:objects 
c1 - collector
p1 - producer
w1 - wood
st1 - stone
s1 - storage
rs1 - r_station
)
(:init 
(= (battery-amount  c1) 15)
(= (battery-amount  p1) 15)
(= (wood-amount  c1) 0)
(= (stone-amount  c1) 0)
(= (wood-amount  p1) 0)
(= (stone-amount  p1) 0)
(= (chest-amount  p1) 0)
(= (posX  c1) 0)
(= (posY  c1) 0)
(= (posX  p1) 1)
(= (posY  p1) 2)
(free  p1)
(free  c1)
(= (wood-stored  s1) 0)
(= (stone-stored  s1) 0)
(= (chest-stored  s1) 0)
(= (posX  w1) 1)
(= (posY  w1) 1)
(= (posX  st1) 1)
(= (posY  st1) 1)
(= (posX  s1) 1)
(= (posY  s1) 1)
(= (posX  rs1) 1)
(= (posY  rs1) 1)
(= (battery-capacity) 100)
(= (sample-capacity) 100)
)
(:goal 
(and 
(= (wood-stored  s1) 1)
(= (stone-stored  s1) 1)
)
)
)
