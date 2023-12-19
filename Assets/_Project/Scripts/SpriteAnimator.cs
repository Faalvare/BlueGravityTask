using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private Sprite spriteSheet;
    [SerializeField] private SpriteAnimation[] animationList;
    [Header("Crop Properties")]
    [SerializeField] private Vector2 cropSize = new Vector2(64,64);
    [SerializeField] private Vector2 pivot = new Vector2(0.5f,0.5f);
    [SerializeField] private int pixelsPerUnit = 64;
    [SerializeField] private CropOrder cropOrder = CropOrder.TopLeftToBottomRight;
    public string animationTest;
    public SpriteAnimation currentSpriteAnimation { get; private set; }
    public AnimationFrame currentAnimationFrame { get; private set; }
    private SpriteRenderer spriteRenderer;
    public List<Sprite> spriteCrops { get; private set; }
    private Coroutine playAnimationCR;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteSheet)
            CropSprite();
    }

    private void OnValidate()
    {
        PlayAnimationByName(animationTest);
    }

    public void SetSprite(Sprite sprite)
    {
        this.spriteSheet = sprite;
        CropSprite();
    }

    /// <summary>
    /// Finds a SpriteAnimation by its name within the animationList.
    /// </summary>
    /// <param name="animationName">The name of the animation to find.</param>
    /// <returns>The SpriteAnimation with the specified name; returns null if not found.</returns>
    public SpriteAnimation FindAnimationByName(string animationName)
    {
        foreach (var item in animationList)
        {
            if (item.name == animationName)
                return item;
        }
        return null;
    }

    /// <summary>
    /// Plays the specified AnimationFrame, updating the spriteRenderer with the frame's sprite.
    /// </summary>
    /// <param name="animationFrame">The AnimationFrame to be played.</param>
    private void PlayAnimationFrame(AnimationFrame animationFrame)
    {
        currentAnimationFrame = animationFrame;
        spriteRenderer.sprite = spriteCrops[animationFrame.frameIndex];
        animationFrame.onFrame?.Invoke();
    }

    /// <summary>
    /// Plays the animation specified by name.
    /// </summary>
    /// <param name="animationName">The name of the animation to play.</param>
    public void PlayAnimationByName(string animationName,bool loop = true)
    {
        SpriteAnimation spriteAnimation = FindAnimationByName(animationName);

        if (spriteAnimation == null)
            return;

        if (playAnimationCR != null)
        {
            currentSpriteAnimation.onAnimationEnd?.Invoke();
            StopCoroutine(playAnimationCR);
        }

        spriteAnimation.onAnimationStart?.Invoke();
        playAnimationCR = StartCoroutine(PlayAnimationCoroutine(spriteAnimation,loop));
    }

    /// <summary>
    /// Crops a sprite sheet into individual sprites based on specified crop size and crop order.
    /// </summary>
    private void CropSprite()
    {
        Vector2 spriteSheetSize = new Vector2(spriteSheet.texture.width, spriteSheet.texture.height);
        Vector2 frameDimensions = new Vector2(spriteSheetSize.x / cropSize.x, spriteSheetSize.y / cropSize.y);
        spriteCrops = new List<Sprite>();
        if (cropOrder == CropOrder.BottomLeftToTopRight)
        {
            for (int y = 0; y < frameDimensions.y; y++)
            {
                for (int x = 0; x < frameDimensions.x; x++)
                {
                    Rect rect = new Rect(new Vector2(cropSize.x * x, cropSize.y * y), cropSize);
                    Debug.Log(rect);
                    spriteCrops.Add(Sprite.Create(spriteSheet.texture, rect, pivot, pixelsPerUnit));
                }
            }
        }
        else
        {
            for (int y = (int)frameDimensions.y-1; y >= 0; y--)
            {
                for (int x = 0; x < frameDimensions.x; x++)
                {
                    Rect rect = new Rect(new Vector2(cropSize.x * x, cropSize.y * y), cropSize);
                    spriteCrops.Add(Sprite.Create(spriteSheet.texture, rect, pivot, pixelsPerUnit));
                }
            }
        }

    }

    /// <summary>
    /// Coroutine for playing an animation defined by the given SpriteAnimation.
    /// </summary>
    /// <param name="spriteAnimation">The SpriteAnimation containing frames to be played.</param>
    /// <param name="loop">Does the animation loop?</param>
    private IEnumerator PlayAnimationCoroutine(SpriteAnimation spriteAnimation,bool loop)
    {
        if (spriteAnimation == null||spriteAnimation.animationFrames.Length==0)
            yield break;

        currentSpriteAnimation = spriteAnimation;
        if (spriteAnimation.animationFrames.Length == 1)
        {
            PlayAnimationFrame(spriteAnimation.animationFrames[0]);
            yield break;
        }

        while (true)
        {
            foreach (var frame in spriteAnimation.animationFrames)
            {
                PlayAnimationFrame(frame);
                yield return new WaitForSeconds(frame.delay * 0.001f);
            }
            if (!loop)
            {
                spriteAnimation.onAnimationEnd?.Invoke();
                yield break;
            }
        }
    }

    [System.Serializable]
    public class SpriteAnimation
    {
        public string name;
        public AnimationFrame[] animationFrames;
        public UnityEvent onAnimationStart;
        public UnityEvent onAnimationEnd;
    }

    [System.Serializable]
    public class AnimationFrame
    {
        [Tooltip("The index of the sprite in the atlas")]
        public int frameIndex;
        [Tooltip("Time in miliseconds between this frame and the next")]
        public int delay;
        public UnityEvent onFrame;
    }

    public enum CropOrder
    {
        TopLeftToBottomRight,
        BottomLeftToTopRight
    }
}
